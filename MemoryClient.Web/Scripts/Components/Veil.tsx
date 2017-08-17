import * as React from "react";

interface IVeilProps {
    zIndex: number;
    opacity: number;
}

class Veil extends React.Component<IVeilProps, undefined>{
    divStyle: {
        "z-index": 100,
        width: "100%",
        height: "100%",
        background: "black",
        top: 0, left: 0,
        position: "fixed",
        opacity: 0;
    }

    render(): JSX.Element {
        return (
            <div id="veil" style={this.divStyle}></div>
        );
    }
}

export default Veil;