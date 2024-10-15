import {
  FC,
  useState,
  useEffect,
  ReactNode,
  cloneElement,
  ReactElement,
} from 'react';

import { BaseProduct } from '../../types/BaseProduct';
import ProductBlock from '../../components/admin/ProductBlock';
import Button from '../../components/shared/Button';

import Style from './ProductsGrid.module.css';

export interface ProductsGridProps<T extends BaseProduct> {
  title: string;
  data: T[];
  pageUrl?: string;
  renderPopUpComponent?: (onClose: () => void) => ReactNode;
  renderFilterComponent?: () => ReactNode;
  deleteProduct?: (id: string) => void;
  updateIsActiveProduct?: (id: string) => void;
}

const ProductsGrid: FC<ProductsGridProps<BaseProduct>> = ({
  title,
  data,
  pageUrl,
  renderPopUpComponent,
  renderFilterComponent,
  deleteProduct,
  updateIsActiveProduct,
}) => {
  const [products, setProducts] = useState<BaseProduct[]>(data);
  const [isPopUpOpen, setIsPopUpOpen] = useState(false);
  const [isOpen, setIsOpen] = useState(true);

  const toggleOpen = () => {
    setIsOpen((prev) => !prev);
  };

  const handlePopUpOpen = () => setIsPopUpOpen(true);
  const handlePopUpClose = () => setIsPopUpOpen(false);

  useEffect(() => {
    setProducts(data);
  }, [data]);

  const getActions = (item: BaseProduct) => {
    const actions = [];
    if (deleteProduct) {
      actions.push({
        label: 'Удалить',
        onClick: () => deleteProduct(item.id.toString()),
      });
    }
    if (updateIsActiveProduct) {
      actions.push({
        label: `Сделать ${item.isActive ? 'неактивной' : 'активной'}`,
        onClick: () => updateIsActiveProduct(item.id.toString()),
      });
    }
    return actions.length > 0 ? actions : undefined;
  };

  return (
    <>
      <div className={Style.candlesGrid}>
        <div className={Style.titleBlock}>
          <div className={Style.sectionHeader}>
            <h2 onClick={toggleOpen}>{title}</h2>
            {renderPopUpComponent && !isOpen && (
              <Button
                text="Добавить"
                onClick={handlePopUpOpen}
                color="#2E67EA"
              />
            )}
          </div>
          <button className={Style.toggleButton} onClick={toggleOpen}>
            {isOpen ? '–' : '+'}
          </button>
        </div>
        {isOpen && (
          <>
            {renderFilterComponent && renderFilterComponent()}
            <div className={Style.grid}>
              {products.map((item: BaseProduct) => (
                <ProductBlock
                  key={item.id}
                  product={item}
                  pageUrl={pageUrl}
                  actions={getActions(item)}
                />
              ))}
              {renderPopUpComponent && (
                <Button
                  text="Добавить"
                  onClick={handlePopUpOpen}
                  color="#2E67EA"
                />
              )}
            </div>
          </>
        )}
      </div>
      {isPopUpOpen &&
        renderPopUpComponent &&
        cloneElement(renderPopUpComponent(handlePopUpClose) as ReactElement, {
          onClose: handlePopUpClose,
        })}
    </>
  );
};

export default ProductsGrid;
