import { FC, useState, useEffect, useRef, useMemo } from 'react';
import { Link } from 'react-router-dom';

import { BaseProduct } from '../../types/BaseProduct';
import IconMoreVertLarge from '../../UI/IconMoreVertLarge';
import ContextMenu, { Action } from './ContextMenu';
import { Image } from '../../types/Image';
import CustomImage from '../shared/Image';

import Style from './ProductBlock.module.css';

export interface ProductBlockProps<T extends BaseProduct> {
  product: T;
  width?: number;
  pageUrl?: string;
  actions?: Action[];
}

const ProductBlock: FC<ProductBlockProps<BaseProduct>> = ({
  product,
  width,
  pageUrl,
  actions,
}) => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);
  const [areThereImages, setAreThereImages] = useState(false);
  const [firstImage, setFirstImage] = useState<Image>(null);

  const handleMenuToggle = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  useEffect(() => {
    const imagesExist = product.images && product.images.length > 0;
    setAreThereImages(imagesExist);
    if (imagesExist) {
      setFirstImage(product.images[0]);
    }
  }, [product.images]);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (ref.current && !ref.current.contains(event.target as Node)) {
        setIsMenuOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [ref]);

  const productBlockStyle = useMemo(
    () => ({
      width: width ? `${width}px` : undefined,
    }),
    [width],
  );

  const productLink = useMemo(
    () => (pageUrl ? `/admin/${pageUrl}/${product.id}` : ''),
    [pageUrl, product.id],
  );

  return (
    <div
      className={`${Style.productBlock} ${
        product.isActive ? '' : Style.isNonActive
      }`}
      style={productBlockStyle}
      ref={ref}
    >
      <div
        className={`${areThereImages ? Style.imageBlock : Style.nonImageBlock}`}
      >
        {firstImage && (
          <CustomImage
            name={firstImage.fileName}
            alt={firstImage.alternativeName}
          />
        )}
      </div>
      <div className={Style.info}>
        <Link to={productLink} className={Style.link}>
          <p className={Style.title}>{product.title}</p>
          <p className={Style.description}>{product.description}</p>
        </Link>
      </div>
      <div className={Style.options}>
        <span
          className={`${Style.activeBlock} ${
            product.isActive ? Style.isActive : ''
          }`}
        ></span>
        {actions && actions.length > 0 && (
          <>
            <button onClick={handleMenuToggle} className={Style.contextMenuBtn}>
              <IconMoreVertLarge color="#777" />
            </button>
            {isMenuOpen && <ContextMenu actions={actions} />}
          </>
        )}
      </div>
    </div>
  );
};

export default ProductBlock;
