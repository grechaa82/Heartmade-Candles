import { FC, useState } from 'react';

import ProductsGridSelector from '../../components/constructor/ProductsGridSelector';
import { CandleTypeWithCandles } from '../../typesV2/CandleTypeWithCandles';

import Style from './CandleSelectionPanel.module.css';

export interface CandleSelectionPanelProps {
  data: CandleTypeWithCandles[];
}

const CandleSelectionPanel: FC<CandleSelectionPanelProps> = ({ data }) => {
  const [candleTypeWithCandles, setCandleTypeWithCandles] = useState<CandleTypeWithCandles[]>(data);

  return (
    <div className={Style.content}>
      {candleTypeWithCandles &&
        candleTypeWithCandles.map((item, index) => (
          <ProductsGridSelector key={index} title={item.type} data={item.candles} />
        ))}
    </div>
  );
};

export default CandleSelectionPanel;
