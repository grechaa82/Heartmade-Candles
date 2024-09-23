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
import ButtonDropdown, {
  optionData,
} from '../../components/shared/ButtonDropdown';

import Style from './ProductsGrid.module.css';

export interface ProductsGridProps<T extends BaseProduct> {
  title: string;
  data: T[];
  pageUrl?: string;
  popUpComponent?: ReactNode;
  filterComponent?: ReactNode;
  filters?: optionData[];
  selectedFilter?: optionData;
  onFiltersSelect?: (filter: optionData) => void;
  deleteProduct?: (id: string) => void;
  updateIsActiveProduct?: (id: string) => void;
}

export type FetchProducts<T extends BaseProduct> = () => Promise<T[]>;

const ProductsGrid: FC<ProductsGridProps<BaseProduct>> = ({
  title,
  data,
  pageUrl,
  popUpComponent,
  filterComponent,
  filters,
  selectedFilter,
  onFiltersSelect,
  deleteProduct,
  updateIsActiveProduct,
}) => {
  const [products, setProducts] = useState<BaseProduct[]>(data);
  const [isPopUpOpen, setIsPopUpOpen] = useState(false);

  const handlePopUpOpen = () => {
    setIsPopUpOpen(true);
  };

  const handlePopUpClose = () => {
    setIsPopUpOpen(false);
  };

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
        onClick: () => updateIsActiveProduct(item.id.toFixed()),
      });
    }

    return actions.length > 0 ? actions : undefined;
  };

  return (
    <div className={Style.candlesGrid}>
      <div className={Style.titleBlock}>
        <h2>{title}</h2>
        {popUpComponent && (
          <ButtonWithIcon
            icon={IconPlusLarge}
            text="Добавить"
            onClick={handlePopUpOpen}
            color="#2E67EA"
          />
        )}
        {filterComponent}
        {filters && onFiltersSelect && (
          <ButtonDropdown
            text={'Тип свечей'}
            options={filters}
            selected={selectedFilter}
            onChange={onFiltersSelect}
          />
        )}
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
        cloneElement(popUpComponent as ReactElement, {
          onClose: handlePopUpClose,
        })}
    </div>
  );
};

export default ProductsGrid;
