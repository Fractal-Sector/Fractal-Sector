## UI
cargo-console-menu-nf-populate-orders-cargo-order-row-product-name-text = {CAPITALIZE($productName)} (x{$total}) для {$purchaser}
cargo-console-menu-nf-populate-orders-cargo-order-row-product-quantity-text = {$remaining} осталось.
cargo-console-menu-nf-order-capacity = {$count}/{$capacity}
cargo-console-order-nf-menu-notes-label = Заметки:

## Orders
cargo-console-nf-no-bank-account = Банковский счет не найден

cargo-console-nf-paper-print-text = [head=2]Заказ #{$orderNumber}[/head]
    {"[bold]Предмет:[/bold]"} {$itemName} ({$orderIndex}/{$orderQuantity})
    {"[bold]Кем куплено:[/bold]"} {$purchaser}
    {"[bold]Заметки:[/bold]"} {$notes}

