import { FC, useState, useEffect } from 'react';
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
  onChanges?: (isChanges: boolean) => void;
  handleChangesProduct?: (updatedItem: T[]) => void;
}

export type FetchProducts<T extends BaseProduct> = () => Promise<T[]>;

const ProductsGrid: FC<ProductsGridProps<BaseProduct>> = ({
  data,
  title,
  fetchProducts,
  onChanges,
  handleChangesProduct,
}) => {
  const [products, setProducts] = useState<BaseProduct[]>(data);
  const [allProducts, setAllProducts] = useState<BaseProduct[]>([]);
  const [selectedProducts, setSelectedProducts] = useState<BaseProduct[]>([]);
  const [openPopUp, setOpenPopUp] = useState(false);
  const [isDataLoaded, setIsDataLoaded] = useState(false);

  const fetchData = async () => {
    if (fetchProducts) {
      const fetchedData = await fetchProducts();
      setAllProducts(fetchedData);
      setIsDataLoaded(true);
    }
  };

  const handleAddProduct = (product: BaseProduct) => {
    const index = selectedProducts.findIndex((p) => p.id === product.id);
    if (index >= 0) {
      return;
    }

    setSelectedProducts([...selectedProducts, product]);

    if (onChanges && handleChangesProduct) {
      onChanges(true);
      handleChangesProduct(products);
    }
  };

  const handleRemoveProduct = (product: BaseProduct) => {
    setSelectedProducts(selectedProducts.filter((p) => p.id !== product.id));

    if (onChanges && handleChangesProduct) {
      onChanges(true);
      handleChangesProduct(products);
    }
  };

  useEffect(() => {
    setProducts(selectedProducts);
  }, [selectedProducts]);

  useEffect(() => {
    setSelectedProducts(data);
    setProducts(data);
  }, [data]);

  return (
    <div className={Style.candlesGrid}>
      <h2>{title}</h2>
      <div className={Style.grid}>
        {products.map((item: BaseProduct) => (
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
          <AddProductPopUp
            title={title}
            allData={allProducts}
            selectedData={selectedProducts}
            onAddProduct={handleAddProduct}
            onRemoveProduct={handleRemoveProduct}
            onClose={() => setOpenPopUp(false)}
            isDataLoaded={isDataLoaded}
          />
        )}
      </div>
    </div>
  );
};

export default ProductsGrid;
