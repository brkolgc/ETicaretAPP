export class SingleOrder {
    address: string;
    createdDate: Date;
    description: string;
    id: string;
    orderCode: string;
    basketItems: BasketItems[];
    // basketItems:any[];
}

export class BasketItems {
    name: string;
    price: number;
    quantity: number;
}