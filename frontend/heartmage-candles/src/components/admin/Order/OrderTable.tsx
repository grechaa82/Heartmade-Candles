import { FC, useEffect, useState } from 'react';

import { OrdersApi } from '../../../services/OrdersApi';
import {
  OrderTableParameters,
  TypesOfSorting,
} from '../../../typesV2/order/OrderTableParametersRequest';
import { Order } from '../../../typesV2/shared/Order';
import {
  getStatusString,
  OrderStatus,
} from '../../../typesV2/order/OrderStatus';
import Pagination from './Pagination';
import FilterOrderTable from './FilterOrderTable';

import Style from './OrderTable.module.css';

interface OrderTableProps {}

interface FilterParams {
  sortBy: TypesOfSorting | null;
  ascending: boolean;
  createdFrom: Date | null;
  createdTo: Date | null;
  status: OrderStatus | null;
  pageSize: number;
  pageIndex: number;
}

const OrderTable: FC<OrderTableProps> = ({}) => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [filterParams, setFilterParams] = useState<FilterParams>({
    sortBy: null,
    ascending: true,
    createdFrom: null,
    createdTo: null,
    status: null,
    pageSize: 10,
    pageIndex: 0,
  });

  useEffect(() => {
    fetchOrders();
  }, [filterParams, setFilterParams]);

  const fetchOrders = async () => {
    const ordersAndTotalCountResponse = await OrdersApi.getAll(
      new OrderTableParameters(filterParams)
    );
    if (
      ordersAndTotalCountResponse.data &&
      !ordersAndTotalCountResponse.error
    ) {
      setOrders(ordersAndTotalCountResponse.data.orders);
      setTotalCount(ordersAndTotalCountResponse.data.totalCount);
      return ordersAndTotalCountResponse.data;
    } else {
      //setErrorMessage([...errorMessage, ordersAndTotalCountResponse.error as string]);
      return [];
    }
  };

  const handleSort = (type: TypesOfSorting) => {
    const newAscending =
      filterParams.sortBy === type ? !filterParams.ascending : true;
    setFilterParams({
      ...filterParams,
      sortBy: type,
      ascending: newAscending,
    });
  };

  const handleFilter = (filters: Partial<FilterParams>) => {
    setFilterParams((prevParams) => ({
      ...prevParams,
      ...filters,
      pageIndex: 0,
    }));
  };

  const handlePageChange = (pageIndex: number) => {
    setFilterParams({
      ...filterParams,
      pageIndex,
    });
  };

  const formatDate = (date: Date) => {
    const day = date.getDate();
    const month = date.toLocaleString('default', { month: 'short' });
    return `${day} ${month}`;
  };

  return (
    <div>
      <div>
        <FilterOrderTable onFilter={handleFilter} />
      </div>
      <table>
        <thead>
          <tr>
            <th>Id</th>
            <th onClick={() => handleSort('createdat')}>Создано</th>
            <th onClick={() => handleSort('updatedat')}>Обновлено</th>
            <th>Строка конф.</th>
            <th onClick={() => handleSort('status')}>Статус</th>
            <th>Разных</th>
            <th>Общее кол.</th>
            <th onClick={() => handleSort('totalprice')}>Сумма</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.id}>
              <td>{order.id.substring(order.id.length - 6)}</td>
              <td>{formatDate(new Date(order.createdAt))}</td>
              <td>{formatDate(new Date(order.createdAt))}</td>
              <td>{order.basket.filterString.substring(0, 10)}..</td>
              <td>{getStatusString(order.status)}</td>
              <td>{order.basket.items.length}</td>
              <td>{order.basket.totalQuantity}</td>
              <td>{order.basket.totalPrice}</td>
            </tr>
          ))}
        </tbody>
      </table>
      <div className={Style.paggination}>
        <Pagination
          totalCount={totalCount}
          pageSize={filterParams.pageSize}
          currentPage={filterParams.pageIndex}
          onPageChange={handlePageChange}
        />
      </div>
    </div>
  );
};

export default OrderTable;
