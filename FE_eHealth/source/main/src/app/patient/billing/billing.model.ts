export interface PaymentDto {
    id: number;
    appointmentId: number;
    amount: number;
    status: string;
    transactionId: string;
}

export interface CreatePaymentCommand {
    appointmentId: number;
    amount: number;
}