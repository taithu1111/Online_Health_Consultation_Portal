export class BillList {
  id: string;
  img: string;
  patientName: string;
  doctorName: string;
  status: string;
  initialAmount: string;
  tax: string;
  date: string;
  discount: string;
  total: string;

  constructor(billList: Partial<BillList> = {}) {
    this.id = billList.id || this.getRandomID();
    this.img = billList.img || 'assets/images/user/user1.jpg';
    this.patientName = billList.patientName || '';
    this.doctorName = billList.doctorName || '';
    this.status = billList.status || '';
    this.initialAmount = billList.initialAmount || '';
    this.tax = billList.tax || '';
    this.date = billList.date || '';
    this.discount = billList.discount || '';
    this.total = billList.total || '';
  }

  public getRandomID(): string {
    const random = Math.floor(10000 + Math.random() * 90000);
    return `BILL-${random}`;;
  }
}

export interface PaymentDto {
  id: number;
  appointmentId: number;
  amount: number;
  status: string;
  transactionId: string;
}
