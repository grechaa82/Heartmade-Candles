import { FC, useState } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoWick from '../../modules/admin/MainInfoWick';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useWickByIdQuery from '../../hooks/useWickByIdQuery';

import Style from './WickPage.module.css';

type WickParams = {
  id: string;
};

const WickPage: FC = () => {
  const { id } = useParams<WickParams>();
  const { data, isLoading, updateWick } = useWickByIdQuery(id);
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  if (isLoading) {
    return <div>...Loading</div>;
  }

  return (
    <>
      <div className="wicks">
        {data && <MainInfoWick data={data} onSave={updateWick} />}
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default WickPage;
