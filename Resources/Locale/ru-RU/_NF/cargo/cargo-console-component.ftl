## UI
cargo-console-menu-nf-populate-orders-cargo-order-row-product-name-text = {CAPITALIZE($productName)} (x{$total}) для {$purchaser}
cargo-console-menu-nf-populate-orders-cargo-order-row-product-quantity-text = {$remaining} слева
cargo-console-menu-nf-order-capacity = {$count}/{$capacity}
cargo-console-order-nf-menu-notes-label = Заметки:

## Orders
cargo-console-nf-no-bank-account = Банковский счет не найден

cargo-console-nf-paper-print-text = [head=2]Заказ #{$orderNumber}[/head]
    {"[bold]Item:[/bold]"} {$itemName} ({$orderIndex} или {$orderQuantity})
    {"[bold]Purchased by:[/bold]"} {$purchaser}
    {"[bold]Notes:[/bold]"} {$notes}

