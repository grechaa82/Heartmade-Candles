import { FC } from 'react';

import ProductsGridSelector from '../../components/constructor/ProductsGridSelector';
import { ImageProduct } from '../../typesV2/shared/BaseProduct';
import { useCandleContext } from '../../contexts/CandleContext';
import { useConstructorContext } from '../../contexts/ConstructorContext';

import Style from './CandleSelectionPanel.module.css';

export interface CandleSelectionPanelProps {}

const CandleSelectionPanel: FC<CandleSelectionPanelProps> = ({}) => {
  const { candlesByType } = useConstructorContext();
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
          />
        ))}
    </div>
  );
};

export default CandleSelectionPanel;
