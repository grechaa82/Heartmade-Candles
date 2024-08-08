import { FC } from 'react';

import ProductsGridSelectorV2 from '../../components/constructor/ProductsGridSelectorV2';
import { ImageProduct } from '../../typesV2/shared/BaseProduct';
import { useCandleContext } from '../../contexts/CandleContext';
import { useConstructorContext } from '../../contexts/ConstructorContext';

import Style from './CandleSelectionPanel.module.css';

export interface CandleSelectionPanelPropsV2 {}

const CandleSelectionPanelV2: FC<CandleSelectionPanelPropsV2> = ({}) => {
  const { candlesByType } = useConstructorContext();
  const { fetchCandleById } = useCandleContext();

  const handleOnSelectProduct = (candle: ImageProduct) => {
    fetchCandleById(candle.id.toString());
  };

  return (
    <div className={Style.content}>
      {candlesByType &&
        candlesByType.map((item, index) => (
          <ProductsGridSelectorV2
            key={index}
            title={item.type}
            data={item.candles}
            onSelectProduct={handleOnSelectProduct}
          />
        ))}
    </div>
  );
};

export default CandleSelectionPanelV2;
