import { FC } from 'react';

import ProductsGridSelector from '../../components/constructor/ProductsGridSelector';
import { ImageProduct } from '../../typesV2/shared/BaseProduct';
import { useCandleContext } from '../../contexts/CandleContext';
import { useConstructorContext } from '../../contexts/ConstructorContext';
import Button from '../../components/shared/Button';

import Style from './CandleSelectionPanel.module.css';

export interface CandleSelectionPanelProps {}

const CandleSelectionPanel: FC<CandleSelectionPanelProps> = ({}) => {
  const { candlesByType, loadMoreCandlesByType } = useConstructorContext();
  const { fetchCandleById } = useCandleContext();

  const handleOnSelectProduct = (candle: ImageProduct) => {
    fetchCandleById(candle.id.toString());
  };

  return (
    <div className={Style.content}>
      {candlesByType &&
        candlesByType.map((item, index) => (
          <ProductsGridSelector
            key={index}
            title={item.type}
            data={item.candles}
            onSelectProduct={handleOnSelectProduct}
          >
            <Button
              className={Style.loadMoreBtn}
              text={item.pageSize ? `+ ${item.pageSize}` : 'Добавить'}
              onClick={() =>
                loadMoreCandlesByType(
                  item.type,
                  item.pageSize,
                  item.pageIndex + 1,
                )
              }
            />
          </ProductsGridSelector>
        ))}
    </div>
  );
};

export default CandleSelectionPanel;
