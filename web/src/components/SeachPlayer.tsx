
type SearchPlayerProps = {
    value: string;
    onChange: (value: string) => void;
};

export function SearchPlayer({ value, onChange }: SearchPlayerProps) {
    return (
        <>
            <input
                name="playerName"
                value={value}
                onChange={e => onChange(e.target.value)}
            />
        </>
    );
}