import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import MainInfoDecors from "../modules/MainInfoDecors";
import { Decor } from "../types/Decor";
import { DecorRequest } from "../types/Requests/DecorRequest";

import { getDecorById, putDecor } from "../Api";

type DecorParams = {
  id: string;
};

const DecorPagePage: FC = () => {
  const { id } = useParams<DecorParams>();
  const [decorData, setDecorData] = useState<Decor>();

  const handleChangesDecor = (updatedDecor: Decor) => {
    setDecorData((prevDecorData) => ({
      ...prevDecorData,
      ...updatedDecor,
    }));
  };

  const updateDecor = (updatedItem: Decor) => {
    if (id) {
      const decorRequest: DecorRequest = {
        title: updatedItem.title,
        description: updatedItem.description,
        price: updatedItem.price,
        imageURL: updatedItem.imageURL,
        isActive: updatedItem.isActive,
      };
      putDecor(id, decorRequest);
    }
  };

  useEffect(() => {
    async function fetchDecors() {
      if (id) {
        const data = await getDecorById(id);
        setDecorData(data);
      }
    }

    fetchDecors();
  }, [id]);

  return (
    <>
      <div className="decors">
        {decorData && (
          <MainInfoDecors
            data={decorData}
            handleChangesDecor={handleChangesDecor}
            onSave={updateDecor}
          />
        )}
      </div>
    </>
  );
};

export default DecorPagePage;
