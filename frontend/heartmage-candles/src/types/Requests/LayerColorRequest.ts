import { Image } from '../Image';

export interface LayerColorRequest {
  title: string;
  description: string;
  pricePerGram: number;
  images: Image[];
  isActive: boolean;
}
