import React from "react";
import { ApiReq } from "../Components/Hooks/ApiReq";
import { Accordion } from "../Components/Accordion/Accordion";
import Button from "../Components/Button/Button";

export function DashboardView(props: any) {
  const [isLoading, setIsLoading] = React.useState(true);
  
  const [projects, setProjects] = React.useState<Project[]>([]);
  const [dirty, setDirty] = React.useState(false);
  

  // use effect axios get to server/api/getAllProjects
  React.useEffect(() => {
    ApiReq("/api/Project/GetAllProjects", "get")
      .then((res) => {
        setProjects(res.data);
        setIsLoading(false);
        setDirty(false);
        console.log(res.data)
      })
  }, [dirty]);

  function CreateProject() {
    ApiReq("/api/Project/Create", "post", 
      {
      name: "string",
      description: "string"
      })
      .then((res) => {
        setDirty(true);
        console.log(res);
      })
  }

  return (
    <div style={{minHeight: "100%", minWidth: "100%"}}>
      <h1>Dashboard</h1>
      <Button onClick={() => CreateProject()} >Add Project</Button>
      <h2>Projects</h2>
      {isLoading ? <p>Loading...</p> : 
      projects.map(project => (
        <div>
          <Accordion header={project.name} body={project.description}/>
        </div>
      ))}
    </div>
  );
}

export type Project = {
  name: string;
  description: string;
}