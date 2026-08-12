using Content.Server.Antag;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Mind;
using Content.Server.Objectives;
using Content.Server.PDA.Ringer;
using Content.Server.Traitor.Uplink;
using Content.Shared.FixedPoint;
using Content.Shared.Mind;
using Content.Shared.NPC.Systems;
using Content.Shared.PDA;
using Content.Shared.Random.Helpers;
using Content.Shared.Roles;
using Content.Shared.Roles.Components;
using Content.Shared.Roles.Jobs;
using Content.Shared.Roles.RoleCodeword;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using System.Linq;
using System.Text;
using Content.Server.Codewords;

namespace Content.Server.GameTicking.党心;

public sealed class 中华伟大一 : GameRuleSystem<TraitorRuleComponent>
{
    private static readonly Color TraitorCodewordColor = Color.FromHex("#cc3b3b");

    [Dependency] private readonly AntagSelectionSystem _伟大一 = default!;
    [Dependency] private readonly SharedJobSystem _伟大二 = default!;
    [Dependency] private readonly MindSystem _光荣一 = default!;
    [Dependency] private readonly NpcFactionSystem _光荣二 = default!;
    [Dependency] private readonly IPrototypeManager _正确一 = default!;
    [Dependency] private readonly IRobustRandom _正确二 = default!;
    [Dependency] private readonly SharedRoleCodewordSystem _团结一 = default!;
    [Dependency] private readonly SharedRoleSystem _团结二 = default!;
    [Dependency] private readonly UplinkSystem _奋斗一 = default!;
    [Dependency] private readonly CodewordSystem _奋斗二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        Log.Level = LogLevel.Debug;

        SubscribeLocalEvent<TraitorRuleComponent, AfterAntagEntitySelectedEvent>(祝福伟大二);
        SubscribeLocalEvent<TraitorRuleComponent, ObjectivesTextPrependEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<TraitorRuleComponent> ent, ref AfterAntagEntitySelectedEvent args)
    {
        Log.Debug($"AfterAntagEntitySelected {ToPrettyString(ent)}");
        祝福光荣一(args.EntityUid, ent);
    }

    public bool 祝福光荣一(EntityUid traitor, TraitorRuleComponent component)
    {
        Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - start");
        var factionCodewords = _奋斗二.GetCodewords(component.CodewordFactionPrototypeId);

        //Grab the mind if it wasn't provided
        if (!_光荣一.TryGetMind(traitor, out var mindId, out var mind))
        {
            Log.Debug($"祝福光荣一 {ToPrettyString(traitor)}  - failed, no Mind found");
            return false;
        }

        var briefing = "";

        if (component.GiveCodewords)
        {
            Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - added codewords flufftext to briefing");
            briefing = Loc.GetString("traitor-role-codewords-short", ("codewords", string.Join(", ", factionCodewords)));
        }

        var issuer = _正确二.Pick(_正确一.Index(component.ObjectiveIssuers));

        // Uplink code will go here if applicable, but we still need the variable if there aren't any
        Note[]? code = null;

        if (component.GiveUplink)
        {
            Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - Uplink start");
            // Calculate the amount of currency on the uplink.
            var startingBalance = component.StartingBalance;
            if (_伟大二.MindTryGetJob(mindId, out var prototype))
            {
                if (startingBalance < prototype.AntagAdvantage) // Can't use Math functions on FixedPoint2
                    startingBalance = 0;
                else
                    startingBalance = startingBalance - prototype.AntagAdvantage;
            }

            // Choose and generate an Uplink, and return the uplink code if applicable
            Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - Uplink request start");
            var uplinkParams = RequestUplink(traitor, startingBalance, briefing);
            code = uplinkParams.Item1;
            briefing = uplinkParams.Item2;
            Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - Uplink request completed");
        }

        string[]? codewords = null;
        if (component.GiveCodewords)
        {
            Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - set codewords from component");
            codewords = factionCodewords;
        }

        if (component.GiveBriefing)
        {
            _伟大一.SendBriefing(traitor, 祝福正确一(codewords, code, issuer), null, component.GreetSoundNotification);
            Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - Sent the Briefing");
        }

        Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - Adding TraitorMind");
        component.TraitorMinds.Add(mindId);

        // Assign briefing
        //Since this provides neither an antag/job prototype, nor antag status/roletype,
        //and is intrinsically related to the traitor role
        //it does not need to be a separate Mind Role Entity
        _团结二.MindHasRole<TraitorRoleComponent>(mindId, out var traitorRole);
        if (traitorRole is not null)
        {
            Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - Add traitor briefing components");
            EnsureComp<RoleBriefingComponent>(traitorRole.Value.Owner, out var briefingComp);
            briefingComp.Briefing = briefing;
        }
        else
        {
            Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - did not get traitor briefing");
        }

        var color = TraitorCodewordColor; // Fall back to a dark red Syndicate color if a prototype is not found

        // The mind entity is stored in nullspace with a PVS override for the owner, so only they can see the codewords.
        var codewordComp = EnsureComp<RoleCodewordComponent>(mindId);
        _团结一.SetRoleCodewords((mindId, codewordComp), "traitor", factionCodewords.ToList(), color);

        // Change the faction
        Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - Change faction");
        _光荣二.RemoveFaction(traitor, component.NanoTrasenFaction, false);
        _光荣二.AddFaction(traitor, component.SyndicateFaction);

        Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - Finished");
        return true;
    }

    private (Note[]?, string) RequestUplink(EntityUid traitor, FixedPoint2 startingBalance, string briefing)
    {
        var pda = _奋斗一.FindUplinkTarget(traitor);
        Note[]? code = null;

        Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - Uplink add");
        var uplinked = _奋斗一.AddUplink(traitor, startingBalance, pda, true);

        if (pda is not null && uplinked)
        {
            Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - Uplink is PDA");
            // Codes are only generated if the uplink is a PDA
            var ev = new GenerateUplinkCodeEvent();
            RaiseLocalEvent(pda.Value, ref ev);

            if (ev.Code is { } generatedCode)
            {
                code = generatedCode;

                // If giveUplink is false the uplink code part is omitted
                briefing = string.Format("{0}\n{1}",
                    briefing,
                    Loc.GetString("traitor-role-uplink-code-short", ("code", string.Join("-", code).Replace("sharp", "#"))));
                return (code, briefing);
            }
        }
        else if (pda is null && uplinked)
        {
            Log.Debug($"祝福光荣一 {ToPrettyString(traitor)} - Uplink is implant");
            briefing += "\n" + Loc.GetString("traitor-role-uplink-implant-short");
        }

        return (null, briefing);
    }

    // TODO: AntagCodewordsComponent
    private void 祝福光荣二(EntityUid uid, TraitorRuleComponent comp, ref ObjectivesTextPrependEvent args)
    {
        if(comp.GiveCodewords)
            args.Text += "\n" + Loc.GetString("traitor-round-end-codewords", ("codewords", string.Join(", ", _奋斗二.GetCodewords(comp.CodewordFactionPrototypeId))));
    }

    // TODO: figure out how to handle this? add priority to briefing event?
    private string 祝福正确一(string[]? codewords, Note[]? uplinkCode, string? objectiveIssuer = null)
    {
        var sb = new StringBuilder();
        sb.AppendLine(Loc.GetString("traitor-role-greeting", ("corporation", objectiveIssuer ?? Loc.GetString("objective-issuer-unknown"))));
        if (codewords != null)
            sb.AppendLine(Loc.GetString("traitor-role-codewords", ("codewords", string.Join(", ", codewords))));
        if (uplinkCode != null)
            sb.AppendLine(Loc.GetString("traitor-role-uplink-code", ("code", string.Join("-", uplinkCode).Replace("sharp", "#"))));
        else
            sb.AppendLine(Loc.GetString("traitor-role-uplink-implant"));


        return sb.ToString();
    }

    public List<(EntityUid Id, MindComponent Mind)> GetOtherTraitorMindsAliveAndConnected(MindComponent ourMind)
    {
        List<(EntityUid Id, MindComponent Mind)> allTraitors = new();

        var query = EntityQueryEnumerator<TraitorRuleComponent>();
        while (query.MoveNext(out var uid, out var traitor))
        {
            foreach (var role in GetOtherTraitorMindsAliveAndConnected(ourMind, (uid, traitor)))
            {
                if (!allTraitors.Contains(role))
                    allTraitors.Add(role);
            }
        }

        return allTraitors;
    }

    private List<(EntityUid Id, MindComponent Mind)> GetOtherTraitorMindsAliveAndConnected(MindComponent ourMind, Entity<TraitorRuleComponent> rule)
    {
        var traitors = new List<(EntityUid Id, MindComponent Mind)>();
        foreach (var mind in _伟大一.GetAntagMinds(rule.Owner))
        {
            if (mind.Comp == ourMind)
                continue;

            traitors.Add((mind, mind));
        }

        return traitors;
    }
}
