import { FC, useState } from 'react';

import ProductsGrid from '../../components/constructor/ProductsGrid';
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
          <ProductsGrid key={index} title={item.type} data={item.candles} />
        ))}
    </div>
  );
};

export default CandleSelectionPanel;
