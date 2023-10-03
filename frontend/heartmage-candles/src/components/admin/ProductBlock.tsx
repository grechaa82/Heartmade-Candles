import { FC, useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';

import { BaseProduct } from '../../types/BaseProduct';
import Checkbox from './Checkbox';
import IconMoreVertLarge from '../../UI/IconMoreVertLarge';
import ContextMenu, { Action } from './ContextMenu';
import { apiUrlToImage } from '../../config';

import Style from './ProductBlock.module.css';

export interface ProductBlockProps<T extends BaseProduct> {
  product: T;
  width?: number;
  pageUrl?: string;
  actions?: Action[];
}

const ProductBlock: FC<ProductBlockProps<BaseProduct>> = ({ product, width, pageUrl, actions }) => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);

  const firstImage = product.images && product.images.length > 0 ? product.images[0] : null;

  const handleMenuToggle = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  const handleClickOutside = (event: MouseEvent) => {
    if (ref.current && !ref.current.contains(event.target as Node)) {
      setIsMenuOpen(false);
    }
  };

  useEffect(() => {
    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const productBlockStyle = {
    ...(width && { width: `${width}px` }),
  };

  const getProductLink = (): string => {
    return pageUrl ? `/admin/${pageUrl}/${product.id}` : '';
  };

  return (
    <div className={Style.productBlock} style={productBlockStyle} ref={ref}>
      <div className={Style.imageBlock}>
        <div className={Style.image}>
          {firstImage && (
            <img src={`${apiUrlToImage}/${firstImage.fileName}`} alt={firstImage.alternativeName} />
          )}
        </div>
      </div>
      <div className={Style.info}>
        <Link to={getProductLink()} className={Style.link}>
          <p className={Style.title}>{product.title}</p>
          <p className={Style.description}>{product.description}</p>
        </Link>
      </div>
      <div className={Style.options}>
        <Checkbox checked={product.isActive} />
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
