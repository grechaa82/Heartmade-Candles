export interface Feedback {
  feedback: feedbackType;
  userName: string;
}

export enum feedbackType {
  Telegram = 'Telegram',
  Instagram = 'Instagram',
  Whatsapp = 'Whatsapp',
}
