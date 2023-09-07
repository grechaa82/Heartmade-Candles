import { Image } from './Image';

export interface BaseProduct {
  id: number;
  title: string;
  description: string;
  images: Image[];
  isActive: boolean;
}
