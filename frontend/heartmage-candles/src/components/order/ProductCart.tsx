import { FC, useState } from 'react';

import { BasketItem } from '../../typesV2/order/BasketItem';
import IconChevronDownLarge from '../../UI/IconChevronDownLarge';
import { apiUrlToImage } from '../../config';

import Style from './ProductCart.module.css';

export interface ProductCartProps {
  product: BasketItem;
}

const ProductCart: FC<ProductCartProps> = ({ product }) => {
  const [showMoreInfo, setShowMoreInfo] = useState(false);

  const firstImage =
    product.configuredCandle.candle.images.length > 0
      ? product.configuredCandle.candle.images[0]
      : null;

  return (
    <div className={Style.orderItem}>
      <div className={Style.mainInfo}>
        <div className={Style.image}>
          {firstImage && (
            <img
              src={`${apiUrlToImage}/${firstImage.fileName}`}
              alt={firstImage.alternativeName}
            />
          )}
        </div>
        <div className={Style.info}>
          <div className={Style.titleBlock}>
            <p className={Style.title}>
              {product.configuredCandle.candle.title}
            </p>
            <button
              className={Style.showMoreInfoBtn}
              type="button"
              onClick={() => setShowMoreInfo(!showMoreInfo)}
            >
              Показать настройки
              <IconChevronDownLarge color="#2e67ea" />
            </button>
          </div>
          <div className={Style.secondaryBlock}>
            <p className={Style.quantity}>{product.quantity} шт</p>
            <p className={Style.price}>{product.price} p</p>
          </div>
        </div>
      </div>
      {showMoreInfo && (
        <div className={Style.paramsBlock}>
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Количество слоев</p>
            <p className={Style.paramsInfo}>
              {product.configuredCandle.numberOfLayer.number}
            </p>
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Цвета слоев</p>
            <div className={Style.paramsListInfo}>
              {product.configuredCandle.layerColors?.map(
                (layerColor, index) => (
                  <div className={Style.paramsListInfoItem} key={index}>
                    <span>{index + 1}</span>
                    <p className={Style.paramsInfo}>{layerColor.title}</p>
                  </div>
                )
              )}
            </div>
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Декор</p>
            {product.configuredCandle.decor && (
              <p className={Style.paramsInfo}>
                {product.configuredCandle.decor.title}
              </p>
            )}
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Запах</p>
            {product.configuredCandle.smell && (
              <p className={Style.paramsInfo}>
                {product.configuredCandle.smell.title}
              </p>
            )}
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Фитиль</p>
            {product.configuredCandle.wick && (
              <p className={Style.paramsInfo}>
                {product.configuredCandle.wick.title}
              </p>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default ProductCart;
