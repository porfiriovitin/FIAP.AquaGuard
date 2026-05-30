interface Props {
  name: string
  role: string
  initials: string
}

export function ProfileHeader({ name, role, initials }: Props) {
  return (
    <section className="flex flex-col items-center gap-2">
      {/* Avatar with status dot */}
      <div className="relative shrink-0">
        <div
          className="size-[96px] rounded-full border-4 border-white
                     shadow-[var(--shadow-xs)] flex items-center justify-center"
          style={{ backgroundColor: 'var(--navy-900)' }}
        >
          <span
            className="text-[28px] font-semibold text-white select-none"
            style={{ fontFamily: 'var(--font-display)' }}
          >
            {initials}
          </span>
        </div>
        {/* Online status dot */}
        <span
          className="absolute bottom-1 right-1 size-[16px] rounded-full
                     border-2 border-white"
          style={{ backgroundColor: 'var(--risk-low)' }}
        />
      </div>

      {/* Name and role */}
      <div className="flex flex-col items-center gap-0.5">
        <span
          className="text-[24px] font-semibold tracking-tight text-[var(--navy-900)]"
          style={{ fontFamily: 'var(--font-display)' }}
        >
          {name}
        </span>
        <span
          className="text-[15px] text-center text-[var(--fg-muted)]"
          style={{ fontFamily: 'var(--font-sans)' }}
        >
          {role}
        </span>
      </div>
    </section>
  )
}
