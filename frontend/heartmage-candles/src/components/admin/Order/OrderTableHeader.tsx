import { FC } from 'react';

import { TypesOfSorting } from '../../../typesV2/order/OrderTableParametersRequest';
import { OrderTableFilterParams } from '../../../typesV2/admin/OrderTableFilterParams';

import Style from './OrderTableHeader.module.css';

interface OrderTableHeaderProps {
  orderTableFilterParams: OrderTableFilterParams;
  sortParams: (type: TypesOfSorting) => void;
}

const OrderTableHeader: FC<OrderTableHeaderProps> = ({
  orderTableFilterParams,
  sortParams,
}) => {
  return (
    <thead className={Style.thead}>
      <tr className={Style.tr}>
        <th>Id</th>
        <th onClick={() => sortParams('createdat')}>
          Создано{' '}
          {orderTableFilterParams.sortBy === 'createdat' ? (
            orderTableFilterParams.ascending ? (
              '↑'
            ) : (
              '↓'
            )
          ) : (
            <span className={Style.vhidden}>W</span>
          )}
        </th>
        <th onClick={() => sortParams('updatedat')}>
          Обновлено{' '}
          {orderTableFilterParams.sortBy === 'updatedat' ? (
            orderTableFilterParams.ascending ? (
              '↑'
            ) : (
              '↓'
            )
          ) : (
            <span className={Style.vhidden}>W</span>
          )}
        </th>
        <th>Код свечей</th>
        <th onClick={() => sortParams('status')}>
          Статус{' '}
          {orderTableFilterParams.sortBy === 'status' ? (
            orderTableFilterParams.ascending ? (
              '↑'
            ) : (
              '↓'
            )
          ) : (
            <span className={Style.vhidden}>W</span>
          )}
        </th>
        <th>Разных</th>
        <th>Общее кол.</th>
        <th onClick={() => sortParams('totalprice')}>
          Сумма{' '}
          {orderTableFilterParams.sortBy === 'totalprice' ? (
            orderTableFilterParams.ascending ? (
              '↑'
            ) : (
              '↓'
            )
          ) : (
            <span className={Style.vhidden}>W</span>
          )}
        </th>
      </tr>
    </thead>
  );
};

export default OrderTableHeader;
