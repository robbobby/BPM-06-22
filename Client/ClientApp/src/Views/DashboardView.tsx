import React, { useEffect } from 'react';
import { ApiReq } from '../Utils/Hooks/ApiReq';
import { Accordion } from '../Components/Accordion/Accordion';
import Button from '../Components/Button/Button';
import { Modal } from '../Components/Modal/Modal';
import { useForm } from '../Utils/Hooks/useForm';

enum ModalContent {
    None,
    CreateProject,
    EditProject
}

export function DashboardView(props: any) {
    const { form, handleInputChange } = useForm({
        name: '',
        description: '',
    });

    const [isLoading, setIsLoading] = React.useState(true);
    const [modalContent, setModalContent] = React.useState(ModalContent.None);

    const [showModal, setShowModal] = React.useState(false);

    const [projects, setProjects] = React.useState<Project[]>([]);
    const [dirty, setDirty] = React.useState(false);

    // use effect axios get to server/api/getAllProjects
    React.useEffect(() => {
        ApiReq('/api/Project/GetAllProjects', 'get')
            .then((res) => {
                setProjects(res.data);
                setIsLoading(false);
                setDirty(false);
                console.log(res.data);
            });
    }, [dirty]);

    function CreateProject(e: any) {
      e.preventDefault()
        ApiReq('/api/Project/Create', 'post', form)
            .then((res) => {
                setDirty(true);
            });
    }

    function setModal(content: ModalContent) {
        setModalContent(content);
        setShowModal(true);
    }

    function getModalContent() {
        switch (modalContent) {
            case ModalContent.CreateProject:
                return (
                    <div>
                        <form>
                            <input
                                placeholder={'Project Name'}
                                value={form.name}
                                type="text"
                                onChange={handleInputChange}
                                name="name"
                            />
                            <input
                                value={form.description}
                                type="text"
                                onChange={handleInputChange}
                                name="description"
                                placeholder={'Project Description'}
                            />
                            <Button onClick={CreateProject}>Create Project</Button>
                        </form>
                        <Button onClick={() => setShowModal(false)}>Cancel</Button>
                    </div>
                );
            case ModalContent.EditProject:
                return (
                    <div>
                        <Button>Edit Project</Button>
                        <Button onClick={() => setShowModal(false)}>Cancel</Button>
                    </div>
                );
        }
    }

    return (
        <div style={{ minHeight: '100%', minWidth: '100%' }}>
            <h1>Dashboard</h1>
            <Button onClick={() => setModal(ModalContent.CreateProject)}>Add Project</Button>
            <Button onClick={() => setModal(ModalContent.EditProject)}>Edit Project</Button>
            <h2>Projects</h2>
            {isLoading ? <p>Loading...</p> :
                projects.map(project => (
                    <div>
                        <Accordion header={project.name} body={project.description}/>
                    </div>
                ))}
            <Modal setShow={setShowModal} show={showModal}>
                {getModalContent()}
            </Modal>
        </div>
    );
}


export type Project = {
    name: string;
    description: string;
}