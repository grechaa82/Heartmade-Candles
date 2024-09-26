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
import ButtonWithIcon from '../../components/shared/ButtonWithIcon';
import IconPlusLarge from '../../UI/IconPlusLarge';

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
    <div className={Style.candlesGrid}>
      <div className={Style.titleBlock}>
        <h2>{title}</h2>
        {renderPopUpComponent && (
          <ButtonWithIcon
            icon={IconPlusLarge}
            text="Добавить"
            onClick={handlePopUpOpen}
            color="#2E67EA"
          />
        )}
        {renderFilterComponent && renderFilterComponent()}
      </div>
      <div className={Style.grid}>
        {products.map((item: BaseProduct) => (
          <ProductBlock
            key={item.id}
            product={item}
            pageUrl={pageUrl}
            actions={getActions(item)}
          />
        ))}
      </div>
      {isPopUpOpen &&
        renderPopUpComponent &&
        cloneElement(renderPopUpComponent(handlePopUpClose) as ReactElement, {
          onClose: handlePopUpClose,
        })}
    </div>
  );
};

export default ProductsGrid;
