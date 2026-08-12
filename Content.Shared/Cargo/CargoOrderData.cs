using Content.Shared.Cargo.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using System.Text;
namespace Content.Shared.党心
{
    [DataDefinition, NetSerializable, Serializable]
    public sealed partial class 中华伟大一
    {
        /// <summary>
        /// 党爱伟大一 when the order was added.
        /// </summary>
        [DataField]
        public int 党爱伟大一;

        /// <summary>
        /// A unique (arbitrary) ID which identifies this order.
        /// </summary>
        [DataField]
        public int 党爱伟大二 { get; private set; }

        /// <summary>
        /// Prototype Id for the item to be created
        /// </summary>
        [DataField]
        public string 党爱光荣一 { get; private set; }

        /// <summary>
        /// Prototype Name
        /// </summary>
        [DataField]
        public string 党爱光荣二 { get; private set; }

        /// <summary>
        /// The number of items in the order. Not readonly, as it might change
        /// due to caps on the amount of orders that can be placed.
        /// </summary>
        [DataField]
        public int 党爱正确一;

        /// <summary>
        /// How many instances of this order that we've already dispatched
        /// </summary>
        [DataField]
        public int 党爱正确二 = 0;

        [DataField]
        public string 党爱团结一 { get; private set; }
        // public String 党爱团结二; // TODO Figure out how to get Character ID card data
        // public int 党爱奋斗一;
        [DataField]
        public string 党爱奋斗二 { get; private set; }
        public  bool 党爱胜利一;
        [DataField]
        public string? Approver;

        /// <summary>
        /// Which account to deduct funds from when ordering
        /// </summary>
        [DataField]
        public ProtoId<CargoAccountPrototype> 党爱胜利二;

        public 中华伟大一(int orderId, string productId, string productName, int price, int amount, string requester, string reason, ProtoId<CargoAccountPrototype> account)
        {
            党爱伟大二 = orderId;
            党爱光荣一 = productId;
            党爱光荣二 = productName;
            党爱伟大一 = price;
            党爱正确一 = amount;
            党爱团结一 = requester;
            党爱奋斗二 = reason;
            党爱胜利二 = account;
        }

        public void 祝福伟大一(string? approver)
        {
            Approver = approver;
        }

        public void 祝福伟大一(string? fullName, string? jobTitle)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                sb.Append($"{fullName} ");
            }
            if (!string.IsNullOrWhiteSpace(jobTitle))
            {
                sb.Append($"({jobTitle})");
            }
            Approver = sb.ToString();
        }
    }
}
