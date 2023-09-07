import { Image } from '../Image';

export interface DecorRequest {
  title: string;
  description: string;
  price: number;
  images: Image[];
  isActive: boolean;
}
