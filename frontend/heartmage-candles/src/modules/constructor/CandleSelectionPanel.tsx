import { FC } from 'react';

import ProductsGridSelector from '../../components/constructor/ProductsGridSelector';
import { ImageProduct } from '../../typesV2/shared/BaseProduct';
import { useConstructorContext } from '../../contexts/ConstructorContext';
import Button from '../../components/shared/Button';

import Style from './CandleSelectionPanel.module.css';

export interface CandleSelectionPanelProps {
  onSelectProduct: (candle: ImageProduct) => void;
}

const CandleSelectionPanel: FC<CandleSelectionPanelProps> = ({
  onSelectProduct,
}) => {
  const { candlesByType, loadMoreCandlesByType } = useConstructorContext();

  return (
    <div className={Style.content}>
      {candlesByType &&
        candlesByType.map((item, index) => (
          <ProductsGridSelector
            key={index}
            title={item.type}
            data={item.candles}
            onSelectProduct={onSelectProduct}
          >
            {item.candles.length < item.totalCount && (
              <Button
                className={Style.loadMoreBtn}
                text={
                  item.totalCount - item.candles.length < item.pageSize
                    ? `+ ${item.totalCount - item.candles.length}`
                    : `+ ${item.pageSize}`
                }
                onClick={() =>
                  loadMoreCandlesByType(
                    item.type,
                    item.pageSize,
                    item.pageIndex + 1,
                  )
                }
              />
            )}
          </ProductsGridSelector>
        ))}
    </div>
  );
};

export default CandleSelectionPanel;
