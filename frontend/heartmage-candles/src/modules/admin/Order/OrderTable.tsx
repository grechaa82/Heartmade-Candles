import { FC, useEffect, useState } from 'react';

import { TypesOfSorting } from '../../../typesV2/order/OrderTableParametersRequest';
import { Order } from '../../../typesV2/shared/Order';
import Pagination from '../../../components/admin/Order/Pagination';
import FilterOrderTable from '../../../components/admin/Order/FilterOrderTable';
import RowsPerPageOptions from '../../../components/admin/Order/RowsPerPageOptions';
import { OrderTableFilterParams } from '../../../typesV2/admin/OrderTableFilterParams';
import OrderTableHeader from '../../../components/admin/Order/OrderTableHeader';
import OrderTableBody from '../../../components/admin/Order/OrderTableBody';
import { OrdersAndTotalCount } from '../../../typesV2/shared/OrdersAndTotalCount';
import { OrderStatus } from '../../../typesV2/order/OrderStatus';
import SearchOrderPopUp from '../PopUp/Order/SearchOrderPopUp';

import Style from './OrderTable.module.css';

interface OrderTableProps {
  fetchOrders: (
    filterParams: OrderTableFilterParams,
  ) => Promise<OrdersAndTotalCount>;
  updateOrderStatus: (orderId: string, newStatus: OrderStatus) => void;
  fetchOrderById: (orderId: string) => Promise<Order>;
}

const OrderTable: FC<OrderTableProps> = ({
  fetchOrders,
  updateOrderStatus,
  fetchOrderById,
}) => {
  const rowsPerPage: number[] = [10, 25, 50, 100];
  const [orders, setOrders] = useState<Order[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [filterParams, setFilterParams] = useState<OrderTableFilterParams>({
    sortBy: null,
    ascending: true,
    createdFrom: null,
    createdTo: null,
    status: null,
    pageSize: rowsPerPage[0],
    pageIndex: 0,
  });
  const [searchOrderPopUp, setSearchOrderPopUp] = useState<boolean>(false);

  async function fetchData() {
    const ordersAndTotalCount: OrdersAndTotalCount = await fetchOrders(
      filterParams,
    );
    setOrders(ordersAndTotalCount.orders);
    setTotalCount(ordersAndTotalCount.totalCount);
  }

  useEffect(() => {
    fetchData();
  }, [filterParams, setFilterParams, orders, setOrders]);

  const handleSort = (type: TypesOfSorting) => {
    setFilterParams({
      ...filterParams,
      sortBy: type,
      ascending: !filterParams.ascending,
    });
  };

  const handleFilter = (filters: Partial<OrderTableFilterParams>) => {
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

  const handlePageSizeChange = (pageSize: number) => {
    setFilterParams({
      ...filterParams,
      pageSize,
    });
  };

  const handleUpdateOrderStatus = (orderId: string, newStatus: OrderStatus) => {
    updateOrderStatus(orderId, newStatus);
    fetchData();
  };

  return (
    <div>
      <div className={Style.tableControlPanel}>
        <FilterOrderTable
          onFilter={handleFilter}
          setOpenPopUp={setSearchOrderPopUp}
        />
        <RowsPerPageOptions
          rows={rowsPerPage}
          selected={filterParams.pageSize}
          onChange={handlePageSizeChange}
        />
      </div>
      <table className={Style.table}>
        <OrderTableHeader
          orderTableFilterParams={filterParams}
          sortParams={handleSort}
        />
        <OrderTableBody
          orders={orders}
          updateOrderStatus={handleUpdateOrderStatus}
        />
      </table>
      <div className={Style.paggination}>
        <Pagination
          totalCount={totalCount}
          pageSize={filterParams.pageSize}
          currentPage={filterParams.pageIndex}
          onPageChange={handlePageChange}
        />
      </div>
      {searchOrderPopUp && (
        <SearchOrderPopUp
          onClose={() => setSearchOrderPopUp(false)}
          fetchOrderById={fetchOrderById}
        />
      )}
    </div>
  );
};

export default OrderTable;
