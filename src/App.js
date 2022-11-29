import { useState } from "react";
import "./App.css";
import Product from "./Components/product";
import InputForm from "./Components/InputForm";

function App() {
  const [buttonPopUp, setButtonPopUp] = useState(false);
  const handleButtonPopUp = () => {
    setButtonPopUp(!buttonPopUp);
  };

  const qualified = [
    {
      key: "01",
      name: "Mr Sazzad",
      car_model: "Hybrid",
      phone: "017283618",
    },
    {
      key: "02",
      name: "Mr Sazzad",
      car_model: "Hybrid",
      phone: "017283618",
    },
    {
      key: "03",
      name: "Mr Sazzad",
      car_model: "Hybrid",
      phone: "017283618",
    },
    {
      key: "04",
      name: "Mr Sazzad",
      car_model: "Hybrid",
      phone: "017283618",
    },
  ];
  return (
    <div className="m-4">
      <button onClick={handleButtonPopUp} className="btn btn-warning mx-5">
        Create
      </button>

      <InputForm trigger={buttonPopUp} />
      <div className="container my-4 p-3">
        <div className="row">
          <div className="col-sm">
            <h5 className="border-3 border-dark border-bottom p-2">New</h5>
            <Product />
          </div>
          <div className="col-sm">
            <h5 className="border-3 border-dark border-bottom p-2">
              Qualified
            </h5>
            {qualified.map((product) => (
              <Product
                key={product.key}
                name={product.name}
                car_model={product.car_model}
                phone={product.phone}
                state={product.state}
              ></Product>
            ))}
          </div>
          <div className="col-sm">
            <h5 className="border-3 border-dark border-bottom p-2">
              Intermediate
            </h5>
            <Product />
          </div>
          <div className="col-sm">
            <h5 className="border-3 border-dark border-bottom p-2">Won</h5>
            <Product />
          </div>
          <div className="col-sm">
            <h5 className="border-3 border-dark border-bottom p-2">
              Delivered
            </h5>
            <Product />
          </div>
          <div className="col-sm">
            <h5 className="border-3 border-dark border-bottom p-2">Lost</h5>
            <Product />
          </div>
        </div>
      </div>
    </div>
  );
}

export default App;
