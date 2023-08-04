import { FC } from 'react';
import { Link } from 'react-router-dom';

import CornerTag from './CornerTag';
import { ImageProduct } from '../../typesV2/BaseProduct';

import Style from './Product.module.css';

export interface ProductProps {
  product: ImageProduct;
  pageUrl?: string;
  onSelectProduct?: (product: ImageProduct) => void;
  onDeselectProduct?: (product: ImageProduct) => void;
  isSelected?: boolean;
}

const Product: FC<ProductProps> = ({
  product,
  pageUrl,
  onSelectProduct,
  onDeselectProduct,
  isSelected = false,
}) => {
  const urlToImage = 'http://localhost:5000/StaticFiles/Images/';
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
      {onSelectProduct ? (
        <button
          className={`${Style.selectBtn} ${isSelected ? Style.selected : ''}`}
          type="button"
          onClick={() => handleSelectProduct()}
        >
          <div className={Style.image}>
            {firstImage && (
              <img src={urlToImage + firstImage.fileName} alt={firstImage.alternativeName} />
            )}
          </div>
        </button>
      ) : (
        <Link to={getProductLink()} className={Style.link}>
          <div className={Style.image}>
            {firstImage && (
              <img src={urlToImage + firstImage.fileName} alt={firstImage.alternativeName} />
            )}
          </div>
        </Link>
      )}
      <div className={Style.price}>{<CornerTag number={product.price} type="price" />}</div>
    </div>
  );
};

export default Product;
