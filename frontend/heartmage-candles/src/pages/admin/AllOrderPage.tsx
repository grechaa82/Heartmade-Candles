import { FC, useState } from 'react';

import OrderTable from '../../modules/admin/Order/OrderTable';
import { OrderTableParameters } from '../../typesV2/order/OrderTableParametersRequest';
import { OrdersApi } from '../../services/OrdersApi';
import { OrderTableFilterParams } from '../../typesV2/admin/OrderTableFilterParams';
import { OrdersAndTotalCount } from '../../typesV2/shared/OrdersAndTotalCount';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';

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

  return (
    <>
      <OrderTable fetchOrders={fetchOrders} />
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AllOrderPage;
