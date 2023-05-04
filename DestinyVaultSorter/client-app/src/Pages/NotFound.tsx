import {Link} from "react-router-dom";
import '../App.css';

function PageNotFound(){
    return(
        <div className="App">
            <header className="App-header">
                <h1>The page you tried to access does not exist.</h1>
                <Link to ="/">Please return to the home Page.</Link>
            </header>
        </div>
    );
}

export default PageNotFound;