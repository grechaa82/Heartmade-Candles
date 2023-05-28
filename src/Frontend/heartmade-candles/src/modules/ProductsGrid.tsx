import { FC } from 'react';
import Style from './ProductsGrid.module.css';
import { BaseProduct } from '../types/BaseProduct';
import ProductBlock from '../components/ProductBlock';
import ButtonWithIcon from '../components/ButtonWithIcon';
import IconPlusLarge from '../UI/IconPlusLarge';

export interface ProductsGridProps<T extends BaseProduct> {
  data: T[];
  title: string;
}

const ProductsGrid: FC<ProductsGridProps<BaseProduct>> = ({ data, title }) => {
  return (
    <div className={Style.candlesGrid}>
      <h2>{title}</h2>
      <div className={Style.grid}>
        {data.map((item: BaseProduct) => (
          <ProductBlock key={item.id} product={item} />
        ))}
        <ButtonWithIcon
          icon={IconPlusLarge}
          text="Добавить"
          onClick={() => console.log('Кнопка "Добавить" была нажата')}
          color="#2E67EA"
        />
      </div>
    </div>
  );
};

export default ProductsGrid;
