import { FC } from 'react';
import { Link } from 'react-router-dom';

import CornerTag from './CornerTag';
import { ImageProduct } from '../../typesV2/Candle';
import Tag from '../shared/Tag';
import IconAlertCircleLarge from '../../UI/IconAlertCircleLarge';
import { apiUrlToImage } from '../../config';

import Style from './Product.module.css';

export interface ProductProps {
  product: ImageProduct;
  pageUrl?: string;
  onSelectProduct?: (product: ImageProduct) => void;
  onDeselectProduct?: (product: ImageProduct) => void;
  isSelected?: boolean;
  index?: number;
}

const Product: FC<ProductProps> = ({
  product,
  pageUrl,
  onSelectProduct,
  onDeselectProduct,
  isSelected = false,
  index,
}) => {
  const firstImage = product.images && product.images.length > 0 ? product.images[0] : null;

  const getProductLink = (): string => {
    return pageUrl ? `/constructor/${pageUrl}/${product.id}` : '';
  };

  const handleSelectProduct = () => {
    if (isSelected) {
      onDeselectProduct && onDeselectProduct(product);
    } else {
      onSelectProduct && onSelectProduct(product);
    }
  };

  return (
    <div className={Style.product}>
      <div className={Style.descriptionWrapper}>
        <button className={Style.descriptionBtn}>
          <IconAlertCircleLarge color="#aaa" />
        </button>
        <div className={Style.descriptionMenu}>
          <p className={Style.descriptionMenuTitle}>{product.title}</p>
          <p className={Style.descriptionMenuDescription}>{product.description}</p>
        </div>
      </div>
      {onSelectProduct ? (
        <button
          className={`${Style.selectBtn} ${isSelected ? Style.selected : ''}`}
          type="button"
          onClick={() => handleSelectProduct()}
        >
          <div className={Style.image}>
            {firstImage && (
              <img
                src={`${apiUrlToImage}/${firstImage.fileName}`}
                alt={firstImage.alternativeName}
              />
            )}
          </div>
        </button>
      ) : (
        <Link to={getProductLink()} className={Style.link}>
          <div className={Style.image}>
            {firstImage && (
              <img
                src={`${apiUrlToImage}/${firstImage.fileName}`}
                alt={firstImage.alternativeName}
              />
            )}
          </div>
        </Link>
      )}
      <div className={Style.indexTag}>
        {index !== undefined && isSelected && (
          <Tag
            tag={{
              id: index,
              text: index.toString(),
            }}
            appearanceTag="primary"
          />
        )}
      </div>
      <div className={Style.price}>{<CornerTag number={product.price} type="price" />}</div>
    </div>
  );
};

export default Product;
