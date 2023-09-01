import { FC, useState } from 'react';

import { CandleDetailWithQuantityAndPrice } from '../../typesV2/BaseProduct';
import IconChevronDownLarge from '../../UI/IconChevronDownLarge';

import Style from './ProductCart.module.css';

export interface ProductCartProps {
  product: CandleDetailWithQuantityAndPrice;
}

const ProductCart: FC<ProductCartProps> = ({ product }) => {
  const [showMoreInfo, setShowMoreInfo] = useState(false);

  const urlToImage = 'http://localhost:5000/StaticFiles/Images/';
  const firstImage =
    product.candleDetail.candle.images.length > 0 ? product.candleDetail.candle.images[0] : null;

  return (
    <div className={Style.orderItem}>
      <div className={Style.mainInfo}>
        <div className={Style.image}>
          {firstImage && (
            <img src={urlToImage + firstImage.fileName} alt={firstImage.alternativeName} />
          )}
        </div>
        <div className={Style.info}>
          <div className={Style.titleBlock}>
            <p className={Style.title}>{product.candleDetail.candle.title}</p>
            <button
              className={Style.showMoreInfoBtn}
              type="button"
              onClick={() => setShowMoreInfo(!showMoreInfo)}
            >
              Показать настройки
              <IconChevronDownLarge color="#2e67ea" />
            </button>
          </div>
          <p className={Style.quantity}>{product.quantity} шт</p>
          <p className={Style.price}>{product.price} p</p>
        </div>
      </div>
      {showMoreInfo && (
        <div className={Style.paramsBlock}>
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Количество слоев</p>
            <p className={Style.paramsInfo}>{product.candleDetail.numberOfLayer.number}</p>
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Цвета слоев</p>
            <div className={Style.paramsListInfo}>
              {product.candleDetail.layerColors?.map((layerColor, index) => (
                <div className={Style.paramsListInfoItem} key={index}>
                  <span>{index + 1}</span>
                  <p className={Style.paramsInfo}>{layerColor.title}</p>
                </div>
              ))}
            </div>
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Декор</p>
            {product.candleDetail.decor && (
              <p className={Style.paramsInfo}>{product.candleDetail.decor.title}</p>
            )}
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Запах</p>
            {product.candleDetail.smell && (
              <p className={Style.paramsInfo}>{product.candleDetail.smell.title}</p>
            )}
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Фитиль</p>
            {product.candleDetail.wick && (
              <p className={Style.paramsInfo}>{product.candleDetail.wick.title}</p>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default ProductCart;
