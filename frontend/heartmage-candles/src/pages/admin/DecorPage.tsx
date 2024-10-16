import { FC, useState, useContext } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoDecor from '../../modules/admin/MainInfoDecor';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useDecorByIdQuery from '../../hooks/admin/useDecorByIdQuery';
import { AuthContext } from '../../contexts/AuthContext';
import MainInfoSkeleton from '../../modules/admin/MainInfoSkeleton';

import Style from './DecorPage.module.css';

type DecorParams = {
  id: string;
};

const DecorPage: FC = () => {
  const { id } = useParams<DecorParams>();
  const { isAuth } = useContext(AuthContext);
  const { data, isLoading, updateDecor } = useDecorByIdQuery(id, isAuth);
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  if (isLoading) {
    return <MainInfoSkeleton />;
  }

  return (
    <>
      <div className="decors">
        {data && <MainInfoDecor data={data} onSave={updateDecor} />}
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default DecorPage;
