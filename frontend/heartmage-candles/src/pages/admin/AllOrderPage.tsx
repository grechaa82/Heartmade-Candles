import { FC } from 'react';

import OrderTable from '../../components/admin/Order/OrderTable';

import Style from './AllOrderPage.module.css';

export interface AllOrderPageProps {}

const AllOrderPage: FC<AllOrderPageProps> = () => {
  return (
    <>
      <OrderTable />
    </>
  );
};

export default AllOrderPage;
