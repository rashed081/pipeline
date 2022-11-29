import { useState } from "react";

const InputForm = (props) => {
  const [inputArr, setInputArr] = useState([]);
  const [inputData, setInputData] = useState({
    name: " ",
    car_model: " ",
    phone: " ",
    state: " ",
  });

  const changeHandle = (e) => {
    setInputData({ ...inputData, [inputData.name]: e.target.value });
  };


  let { name, car_model, phone, state } = inputData;

  const changeSubmit = (e) => {
    setInputArr([...inputArr, { name, car_model, phone, state }]); 
    console.log(inputArr);
    console.log(inputData);
    e.preventDefault();
    setInputData({name:"", car_model:"", phone:"", state:""});
  };
  return (props.trigger) ? (
    <div className="col-md-6 border border-success rounded p-4 mx-auto">
      <form onSubmit={changeSubmit}>
        <div className="form-row">
          <div className="form-group">
            <label htmlFor="name">Name</label>
            <input
              type="text"
              autoComplete="off"
              className="form-control"
              id="name"
              //value={inputData.name}
              onChange={changeHandle}
              placeholder="Name"
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="model">Car Model</label>
            <input
              type="text"
              autoComplete="off"
              className="form-control"
              id="model"
              //value={inputData.car_model}
              onChange={changeHandle}
              placeholder="Model"
              required
            />
          </div>
        </div>
        <div className="form-group">
          <label htmlFor="phone">Phone</label>
          <input
            type="text"
            autoComplete="off"
            className="form-control"
            id="phone"
            //value={inputData.phone}
            onChange={changeHandle}
            placeholder="Phone"
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="inputState">State</label>
          <select
            id="inputState"
            className="form-control"
            required
            //value={inputData.state}
            onChange={changeHandle}
          >
            <option defaultValue>Choose....</option>
            <option>New</option>
            <option>Qualified</option>
            <option>Intermediate</option>
            <option>Won</option>
            <option>Delivered</option>
            <option>Lost</option>
          </select>
        </div>

        <button
        onClick={()=>props.setTrigger(false)}
          className="btn btn-success mt-2"
        >
          Save
        </button>
      </form>
    </div>
  ) : (
    ""
  );
};
export default InputForm;
