
import React from "react";
import VaultPage from "./Pages/VaultPage";
import {BrowserRouter as Router, Route, Routes} from "react-router-dom";

function App() {
  return (
      <Router>
        <Routes>
          <Route path="" element={<VaultPage/>} />
        </Routes>
      </Router>
  );
}

export default App;
