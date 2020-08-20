import {Component} from "react";
import * as React from "react";
import {Alert} from "reactstrap";

export interface Props {

}

interface State {

}

export class About extends Component<Props, State>{
    render(){
        return (
            <div className="About">
                <h1>About This Site</h1>
    
                <p>This is a tool for players of <a href="https://www.elitedangerous.com/">Elite Dangerous</a>, explorers traversing the galaxy.</p>
                
                <Alert color="primary">
                    <strong>Elite Dangerous Scenic Route Finder v1.2.2</strong>
                </Alert>
                
                <h3>What does it do?</h3>
                <p>
                    If you are at a point A (let's say Sol, home of Earth) and you plan to visit point B (let's say the far-flung Colonia settlement), you might pass by many 
                    other interesting places en route without even knowing. If you don't mind a few extra jumps to see some spectacular interstellar sights, use
                    this tool to find them!</p>
                <p>Simply type in your current position and destination (either the system name as it appears in Elite Dangerous or the Point of Interest name as known
                    by <a href="https://www.edsm.net">EDSM</a>), how far your ship can jump, and how many extra jumps you are willing to consider, and click Search.</p>
                <p>Please note that numbers of jumps are estimated by straight line distance, and so may appear less than the actual number of jumps for ships with low jump range
                    or sparse areas of the galaxy.</p>
                
                <h3>Who wrote it?</h3>
                <p>This tool was written by <strong>Keir MacDonald</strong>, a .NET software developer based in the South of England, occasional Elite Dangerous ship commander, and
                    loyal supporter of the <a href="http://elite-dangerous.wikia.com/wiki/Empire">Empire.</a> You can find contact details on my <a href="https://kamd.me.uk">personal website</a>.</p>
                
                <h3>How does it work?</h3>
                <p>This project uses a React + TypeScript front-end, with an ASP.NET Core backend. The Elite Dangerous galaxy data it uses comes from
                    the <a href="https://www.edsm.net">Elite Dangerous Star Map</a> project, without which this tool would not be possible.</p>
                <p>The source code behind both client and server can be found on my <a href="https://github.com/kamd/EDScenicRoute">GitHub</a>.</p>
            </div>
        );
    }
}

export default About