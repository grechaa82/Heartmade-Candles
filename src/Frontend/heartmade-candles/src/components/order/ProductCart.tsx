import { FC, useState, useEffect } from 'react';

import { CandleDetailWithQuantity } from '../../typesV2/BaseProduct';
import IconChevronDownLarge from '../../UI/IconChevronDownLarge';
import { calculatePrice } from '../../helpers/CalculatePrice';

import Style from './ProductCart.module.css';

export interface ProductCartProps {
  product: CandleDetailWithQuantity;
}

const ProductCart: FC<ProductCartProps> = ({ product }) => {
  const [showMoreInfo, setShowMoreInfo] = useState(false);
  const [price, setPrice] = useState(0);

  useEffect(() => {
    setPrice(calculatePrice(product) * product.quantity);
  }, []);

  const urlToImage = 'http://localhost:5000/StaticFiles/Images/';

  return (
    <div className={Style.orderItem}>
      <div className={Style.mainInfo}>
        <div className={Style.image}>
          <img
            src={urlToImage + product.candle.images[0].fileName}
            alt={product.candle.images[0].alternativeName}
          />
        </div>
        <div className={Style.info}>
          <div className={Style.titleBlock}>
            <p className={Style.title}>{product.candle.title}</p>
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
          <p className={Style.price}>{price} p</p>
        </div>
      </div>
      {showMoreInfo && (
        <div className={Style.paramsBlock}>
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Количество слоев</p>
            {product.numberOfLayers && (
              <p className={Style.paramsInfo}>{product.numberOfLayers[0].number}</p>
            )}
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Цвета слоев</p>
            <div className={Style.paramsListInfo}>
              {product.layerColors?.map((layerColor, index) => (
                <div className={Style.paramsListInfoItem}>
                  <span>{index + 1}</span>
                  <p className={Style.paramsInfo}>{layerColor.title}</p>
                </div>
              ))}
            </div>
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Декор</p>
            {product.decors && <p className={Style.paramsInfo}>{product.decors[0].title}</p>}
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Запах</p>
            {product.smells && <p className={Style.paramsInfo}>{product.smells[0].title}</p>}
          </div>
          <span className={Style.separator} />
          <div className={Style.params}>
            <p className={Style.paramsTitle}>Фитиль</p>
            {product.wicks && <p className={Style.paramsInfo}>{product.wicks[0].title}</p>}
          </div>
        </div>
      )}
    </div>
  );
};

export default ProductCart;
