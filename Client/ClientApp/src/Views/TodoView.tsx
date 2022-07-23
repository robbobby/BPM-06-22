import Button, { ButtonVariant } from "../Components/Button/Button";

export function TodoView() {
  return (
    <div>
      <Button variant={ButtonVariant.Link}>Link</Button>
      <Button variant={ButtonVariant.Transparent}>Transparent</Button>
      <Button variant={ButtonVariant.Info}>Info</Button>
      <Button variant={ButtonVariant.Danger}>Danger</Button>
      <Button variant={ButtonVariant.Primary}>Primary</Button>
      <Button variant={ButtonVariant.Warning}>Warning</Button>
      <Button variant={ButtonVariant.Success}>Success</Button>
      <Button variant={ButtonVariant.Secondary}>Secondary</Button>
      <h1>Todo</h1>
    </div>
  );
}