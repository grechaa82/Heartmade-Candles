import {
  FC,
  useState,
  useEffect,
  ReactNode,
  cloneElement,
  ReactElement,
} from "react";

import { BaseProduct } from "../../types/BaseProduct";
import ProductBlock from "../../components/admin/ProductBlock";
import ButtonWithIcon from "../../components/admin/ButtonWithIcon";
import IconPlusLarge from "../../UI/IconPlusLarge";

import Style from "./ProductsGrid.module.css";

export interface ProductsGridProps<T extends BaseProduct> {
  title: string;
  data: T[];
  pageUrl?: string;
  popUpComponent?: ReactNode;
  deleteProduct?: (id: string) => void;
}

export type FetchProducts<T extends BaseProduct> = () => Promise<T[]>;

const ProductsGrid: FC<ProductsGridProps<BaseProduct>> = ({
  title,
  data,
  pageUrl,
  popUpComponent,
  deleteProduct,
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

  return (
    <div className={Style.candlesGrid}>
      <h2>{title}</h2>
      <div className={Style.grid}>
        {products.map((item: BaseProduct) => (
          <ProductBlock
            key={item.id}
            product={item}
            pageUrl={pageUrl}
            actions={
              deleteProduct
                ? [
                    {
                      label: "Удалить",
                      onClick: () => deleteProduct(item.id.toString()),
                    },
                  ]
                : []
            }
          />
        ))}
        {popUpComponent && (
          <ButtonWithIcon
            icon={IconPlusLarge}
            text="Добавить"
            onClick={handlePopUpOpen}
            color="#2E67EA"
          />
        )}
      </div>
      {isPopUpOpen &&
        cloneElement(popUpComponent as ReactElement, {
          onClose: handlePopUpClose,
        })}
    </div>
  );
};

export default ProductsGrid;
