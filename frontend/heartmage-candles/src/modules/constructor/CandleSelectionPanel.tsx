import { FC } from 'react';

import ProductsGridSelector from '../../components/constructor/ProductsGridSelector';
import { CandleTypeWithCandles } from '../../typesV2/constructor/CandleTypeWithCandles';
import { ImageProduct } from '../../typesV2/shared/BaseProduct';

import Style from './CandleSelectionPanel.module.css';

export interface CandleSelectionPanelProps {
  data: CandleTypeWithCandles[];
  onSelectCandle: (product: ImageProduct) => void;
}

const CandleSelectionPanel: FC<CandleSelectionPanelProps> = ({ data, onSelectCandle }) => {
  return (
    <div className={Style.content}>
      {data &&
        data.map((item, index) => (
          <ProductsGridSelector
            key={index}
            title={item.type}
            data={item.candles}
            onSelectProduct={onSelectCandle}
          />
        ))}
    </div>
  );
};

export default CandleSelectionPanel;
