import { FC } from 'react';

import ProductsGridSelector from '../../components/constructor/ProductsGridSelector';
import { ImageProduct } from '../../typesV2/shared/BaseProduct';

import Style from './CandleSelectionPanel.module.css';
import { CandlesByType } from '../../typesV2/constructor/CandlesByType';

export interface CandleSelectionPanelProps {
  data: CandlesByType[];
  onSelectCandle: (product: ImageProduct) => void;
}

const CandleSelectionPanel: FC<CandleSelectionPanelProps> = ({
  data,
  onSelectCandle,
}) => {
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
