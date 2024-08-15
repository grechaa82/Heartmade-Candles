import { FC, useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';

import { ImageProduct } from '../../typesV2/shared/BaseProduct';
import Tag from '../shared/Tag';
import IconAlertCircleLarge from '../../UI/IconAlertCircleLarge';
import InfoBlockPopUp from './InfoBlockPopUp';
import Picture, { SourceSettings } from '../shared/Picture';

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
  const [showInfoBlockPopup, setShowInfoBlockPopup] = useState(false);
  const elementRef = useRef<HTMLDivElement>(null);
  const [position, setPosition] = useState({ x: 0, y: 0 });

  const firstImage =
    product.images && product.images.length > 0 ? product.images[0] : null;

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

  const handleMouseOver = () => {
    setShowInfoBlockPopup(true);
    const element = elementRef.current;
    if (element) {
      const rect = element.getBoundingClientRect();
      const x = rect.left;
      const y = rect.top;
      setPosition({ x, y });
    }
  };

  const handleMouseLeave = () => {
    setShowInfoBlockPopup(false);
  };

  useEffect(() => {
    const handleScroll = () => {
      handleMouseLeave();
    };

    if (showInfoBlockPopup) {
      document.addEventListener('scroll', handleScroll);
    } else {
      document.removeEventListener('scroll', handleScroll);
    }

    return () => {
      document.removeEventListener('scroll', handleScroll);
    };
  }, [showInfoBlockPopup]);

  const sourceSettings: SourceSettings[] = [
    {
      size: 'small',
      media: '(max-width: 440px)',
    },
    {
      size: 'medium',
      media: '(max-width: 480px)',
    },
    {
      size: 'small',
      media: '(max-width: 648px)',
    },
    {
      size: 'medium',
      media: '(max-width: 720px)',
    },
    {
      size: 'small',
      media: '(max-width: 856px)',
    },
    {
      size: 'medium',
      media: '(max-width: 1101px)',
    },
    {
      size: 'small',
      media: '(max-width: 2150px)',
    },
    {
      size: 'medium',
      media: '(min-width: 2150px)',
    },
  ];

  return (
    <div className={`${Style.product} ${Style.withBackground}`}>
      {onSelectProduct ? (
        <button
          className={`${Style.selectBtn} ${isSelected ? Style.selected : ''}`}
          type="button"
          onClick={() => handleSelectProduct()}
        >
          {firstImage && (
            <Picture
              name={firstImage.fileName}
              alt={firstImage.alternativeName}
              className={Style.overflowHidden}
              sourceSettings={sourceSettings}
            />
          )}
        </button>
      ) : (
        <Link to={getProductLink()} className={Style.link}>
          {firstImage && (
            <Picture
              name={firstImage.fileName}
              alt={firstImage.alternativeName}
              className={Style.overflowHidden}
              sourceSettings={sourceSettings}
            />
          )}
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
      <div className={Style.infoBlock}>
        <p className={Style.price}>
          {product.price}
          <span> р</span>
        </p>
        <div
          className={Style.descriptionWrapper}
          onMouseEnter={handleMouseOver}
          onMouseLeave={handleMouseLeave}
          ref={elementRef}
        >
          <button className={Style.descriptionBtn}>
            <IconAlertCircleLarge color="#aaa" />
          </button>
          {showInfoBlockPopup && (
            <InfoBlockPopUp
              title={product.title}
              description={product.description}
              x={position.x}
              y={position.y}
            />
          )}
        </div>
      </div>
    </div>
  );
};

export default Product;
