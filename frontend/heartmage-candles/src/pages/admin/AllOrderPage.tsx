import { FC, useState } from 'react';

import OrderTable from '../../modules/admin/Order/OrderTable';
import { OrderTableParameters } from '../../typesV2/order/OrderTableParametersRequest';
import { OrdersApi } from '../../services/OrdersApi';
import { OrderTableFilterParams } from '../../typesV2/admin/OrderTableFilterParams';
import { OrdersAndTotalCount } from '../../typesV2/shared/OrdersAndTotalCount';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import { OrderStatus } from '../../typesV2/order/OrderStatus';

import Style from './AllOrderPage.module.css';

export interface AllOrderPageProps {}

const AllOrderPage: FC<AllOrderPageProps> = () => {
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const fetchOrders = async (
    filterParams: OrderTableFilterParams
  ): Promise<OrdersAndTotalCount> => {
    const ordersAndTotalCountResponse = await OrdersApi.getAll(
      new OrderTableParameters(filterParams)
    );
    if (
      ordersAndTotalCountResponse.data &&
      !ordersAndTotalCountResponse.error
    ) {
      return ordersAndTotalCountResponse.data;
    } else {
      setErrorMessage([
        ...errorMessage,
        ordersAndTotalCountResponse.error as string,
      ]);
      return { orders: [], totalCount: 0 };
    }
  };

  const updateOrderStatus = async (orderId: string, newStatus: OrderStatus) => {
    const response = await OrdersApi.updateStatus(orderId, newStatus);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    }
  };

  return (
    <>
      <OrderTable
        fetchOrders={fetchOrders}
        updateOrderStatus={updateOrderStatus}
      />
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AllOrderPage;
