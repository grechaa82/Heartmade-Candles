import { Image } from '../Image';

export interface WickRequest {
  title: string;
  description: string;
  price: number;
  images: Image[];
  isActive: boolean;
}
