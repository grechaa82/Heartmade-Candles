import React, { FC } from 'react';
import Checkbox from './Checkbox';
import CandleData from '../pages/CandlePage';
import Style from './ProductBlock.module.css';

export interface CandleData {
  id: number;
  title: string;
  description: string;
  price: number;
  weightGrams: number;
  imageURL: string;
  isActive: boolean;
  typeCandle: number;
  createdAt: string;
}

export interface ProductBlockProps {
  candleData: CandleData;
  width?: number;
}

const ProductBlock: React.FC<ProductBlockProps> = ({ candleData, width }) => {
  const productBlockStyle = {
    ...(width && { width: `${width}px` }),
  };

  return (
    <div className={Style.productBlock} style={productBlockStyle}>
      <div className={Style.image}></div>
      <div className={Style.info}>
        <p className={Style.title}>{candleData.title}</p>
        <p className={Style.description}>{candleData.description}</p>
      </div>
      <Checkbox checked={candleData.isActive} />
    </div>
  );
};

export default ProductBlock;
