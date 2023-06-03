import { FC, useState } from 'react';
import Style from './ProductsGrid.module.css';
import { BaseProduct } from '../types/BaseProduct';
import ProductBlock from '../components/ProductBlock';
import ButtonWithIcon from '../components/ButtonWithIcon';
import IconPlusLarge from '../UI/IconPlusLarge';
import AddProductPopUp from '../components/AddProductPopUp';

export interface ProductsGridProps<T extends BaseProduct> {
  data: T[];
  title: string;
  fetchProducts?: FetchProducts<T>;
}

export type FetchProducts<T extends BaseProduct> = () => Promise<T[]>;

const ProductsGrid: FC<ProductsGridProps<BaseProduct>> = ({ data, title, fetchProducts }) => {
  const [products, setProducts] = useState<BaseProduct[]>([]);
  const [openPopUp, setOpenPopUp] = useState(false);

  const fetchData = async () => {
    if (fetchProducts) {
      const fetchedData = await fetchProducts();
      setProducts(fetchedData);
    }
  };

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
          onClick={async () => {
            await fetchData();
            setOpenPopUp(true);
          }}
          color="#2E67EA"
        />
        {openPopUp && (
          <AddProductPopUp title={title} data={products} onClose={() => setOpenPopUp(false)} />
        )}
      </div>
    </div>
  );
};

export default ProductsGrid;
